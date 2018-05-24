import React, { Component } from 'react';
import LatestSuiteResults from './LatestSuiteResults'

class LatestSuiteResultsRouter extends Component {
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
      <LatestSuiteResults grouped={this.grouped} debug={this.debug} />
    );
  }
}

export default LatestSuiteResultsRouter;