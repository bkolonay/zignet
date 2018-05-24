import React, { Component } from 'react';
import Home from './Home'
import { getSuiteResults } from '../api/api.js';

class HomeRouter extends Component {
  constructor(props) {
    super(props);

    this.grouped = false;
    this.debug = false;

    if (this.props.queryString.indexOf('group=true') !== -1)
      this.grouped = true;
    if (this.props.queryString.indexOf('debug=true') !== -1)
      this.debug = true;
  }  

  render() {
    return (
      <Home grouped={this.grouped} getResults={() => getSuiteResults(this.grouped, this.debug)} />
    );
  }
}

export default HomeRouter;