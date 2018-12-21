import React, { Component } from 'react';
import { getUrlParams } from '../common/routing/SearchFilter.js'

class ChartButton extends Component {

  render() {
    return (
      <a href={"/?" + getUrlParams(this.props.filter)} className="btn btn-primary float-right" role="button" title="View results as charts">Chart</a>
    );
  }
}

export default ChartButton;
