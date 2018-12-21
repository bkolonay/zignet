import React, { Component } from 'react';
import { getUrlParams } from '../common/routing/SearchFilter.js'

class ListButton extends Component {

  render() {
    return (
      <a href={"/list?" + getUrlParams(this.props.filter)} className="btn btn-primary" role="button" title="View results as list">List</a>
    );
  }
}

export default ListButton;
